using Xunit;
using Microsoft.AspNetCore.Mvc;
using SentimentApi.Controllers;
using SentimentApi.Data;
using SentimentApi.Dtos;
using SentimentApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SentimentApi.Tests
{
    // Suponiendo que SentimentAnalyzer está en este namespace
    using SentimentApi.Services; 

    public class CommentsControllerTests
    {
        private readonly CommentsController _controller;
        private readonly AppDbContext _context;
        private readonly SentimentAnalyzer _sentimentAnalyzer;

        public CommentsControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new AppDbContext(options);

            // Crear instancia simple de SentimentAnalyzer para pruebas
            _sentimentAnalyzer = new SentimentAnalyzer();

            _controller = new CommentsController(_context, _sentimentAnalyzer);
        }

        [Fact]
        public async Task Create_ShouldAddComment()
        {
            var dto = new CreateCommentDto
            {
                ProductId = "p1",
                CommentText = "Excelente producto",
                UserId = "u1"
            };

            var result = await _controller.Create(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var comment = Assert.IsType<Comment>(createdResult.Value);
            Assert.Equal("positivo", comment.Sentiment);
            Assert.Equal(dto.ProductId, comment.ProductId);
            Assert.Equal(dto.CommentText, comment.CommentText);
            Assert.Equal(dto.UserId, comment.UserId);
        }

        [Fact]
        public async Task GetAll_ShouldReturnComments()
        {
            // Arrange: Agregar comentario a la DB en memoria
            _context.Comments.Add(new Comment
            {
                ProductId = "p1",
                CommentText = "Buen producto",
                UserId = "u1",
                Sentiment = "positivo"
            });
            await _context.SaveChangesAsync();

            // Act: Llamar a GetAll sin filtros
            var result = await _controller.GetAll(null, null);

            // Assert: Validar resultado OK con comentarios
            var okResult = Assert.IsType<OkObjectResult>(result);
            var comments = Assert.IsAssignableFrom<IEnumerable<Comment>>(okResult.Value);
            Assert.NotEmpty(comments);
        }
    }
}
