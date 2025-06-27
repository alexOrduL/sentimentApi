using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SentimentApi.Data;
using SentimentApi.Models;

namespace SentimentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SentimentAnalyzer _sentimentAnalyzer;


        public CommentsController(AppDbContext context, SentimentAnalyzer sentimentAnalyzer)
        {
            _context = context;
            _sentimentAnalyzer = sentimentAnalyzer;
        }

        private string AnalyzeSentiment(string text)
        {
            var lower = text.ToLower();
            if (lower.Contains("excelente") || lower.Contains("genial") || lower.Contains("fantástico") || lower.Contains("bueno") || lower.Contains("increíble"))
                return "positivo";
            if (lower.Contains("malo") || lower.Contains("terrible") || lower.Contains("problema") || lower.Contains("defecto") || lower.Contains("horrible"))
                return "negativo";
            return "neutral";
        }

       [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentDto dto)
        {
            var sentiment = _sentimentAnalyzer.Analyze(dto.CommentText);

            var comment = new Comment
            {
                ProductId = dto.ProductId,
                CommentText = dto.CommentText,
                Sentiment = sentiment
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetById(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
                return NotFound();

            return Ok(comment);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? product_id, [FromQuery] string? sentiment)
        {
            var query = _context.Comments.AsQueryable();

            if (!string.IsNullOrEmpty(product_id))
                query = query.Where(c => c.ProductId == product_id);

            if (!string.IsNullOrEmpty(sentiment))
                query = query.Where(c => c.Sentiment == sentiment);

            var results = await query.OrderByDescending(c => c.CreatedAt).ToListAsync();
            return Ok(results);
        }

        [HttpGet("/api/sentiment-summary")]
        public async Task<IActionResult> GetSummary()
        {
            var total = await _context.Comments.CountAsync();
            var grouped = await _context.Comments
                .GroupBy(c => c.Sentiment)
                .Select(g => new { Sentiment = g.Key, Count = g.Count() })
                .ToListAsync();

            var sentimentCounts = grouped.ToDictionary(g => g.Sentiment, g => g.Count);

            return Ok(new {
                total_comments = total,
                sentiment_counts = sentimentCounts
            });
        }
    }
}
