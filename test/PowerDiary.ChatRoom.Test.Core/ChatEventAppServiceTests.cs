using Moq;
using PowerDiary.ChatRoom.Core.Application;
using PowerDiary.ChatRoom.Core.Domain.Entities;
using PowerDiary.ChatRoom.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace PowerDiary.ChatRoom.Test.Core
{
    public class ChatEventAppServiceTests
    {
        private readonly Mock<ChatEventRepository> _repository;
        private readonly ChatEventAppService appService;

        public ChatEventAppServiceTests()
        {
            _repository = new Mock<ChatEventRepository>();
            appService = new ChatEventAppService(_repository.Object);
        }

        [Fact]
        public void GetChatEventsMinuteByMinute_ReturnsEmptyList()
        {
            // arrange
            _repository.Setup(r => r.GetAll(It.IsAny<Expression<Func<ChatEvent, bool>>>())).Returns(Enumerable.Empty<ChatEvent>());

            // act
            var chartEvents = appService.GetChatEventsMinuteByMinute();

            // assert
            Assert.Empty(chartEvents);
        }

        [Fact]
        public void GetChatEventsMinuteByMinute_ReturnsChatEvents()
        {
            // arrange
            var mockedChatEvents = new List<Mock<ChatEvent>>
            {
                new Mock<ChatEvent>(),
                new Mock<ChatEvent>(),
                new Mock<ChatEvent>()
            };

            foreach (var mockedChatEvent in mockedChatEvents)
            {
                mockedChatEvent.SetupGet(c => c.Time).Returns(TimeOnly.MaxValue);
                mockedChatEvent.SetupGet(c => c.User.Name).Returns("Random");
            }

            _repository.Setup(r => r.GetAll(It.IsAny<Expression<Func<ChatEvent, bool>>>())).Returns(mockedChatEvents.Select(c => c.Object));

            // act
            var chartEvents = appService.GetChatEventsMinuteByMinute();

            // assert
            Assert.NotEmpty(chartEvents);
        }
    }
}