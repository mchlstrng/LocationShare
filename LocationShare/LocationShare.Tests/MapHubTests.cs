using LocationShare.Hubs;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace LocationShare.Tests
{
    [TestClass]
    public class MapHubTests
    {
        [TestMethod]
        public async Task UpdateLocation_BroadcastsLocationUpdate()
        {
            var mockClients = new Mock<IHubCallerClients>();
            var mockClientProxy = new Mock<IClientProxy>();
            mockClients.Setup(m => m.Group(It.IsAny<string>())).Returns(mockClientProxy.Object);

            var mockGroups = new Mock<IGroupManager>();
            var mockContext = new Mock<HubCallerContext>();

            var hub = new MapHub
            {
                Clients = mockClients.Object,
                Groups = mockGroups.Object,
                Context = mockContext.Object
            };

            await hub.UpdateLocation("room", "user", 1.0, 1.0);

            mockClients.Verify(clients => clients.Group("room"), Times.Once);
            mockClientProxy.Verify(client => client.SendCoreAsync("ReceiveLocationUpdate", new object[] { "user", 1.0, 1.0 }, default), Times.Once);
        }

        [TestMethod]
        public async Task JoinRoom_AddsConnectionToGroup()
        {
            var mockClients = new Mock<IHubCallerClients>();
            var mockGroups = new Mock<IGroupManager>();
            var mockContext = new Mock<HubCallerContext>();
            mockContext.Setup(m => m.ConnectionId).Returns("connectionId");

            var hub = new MapHub
            {
                Clients = mockClients.Object,
                Groups = mockGroups.Object,
                Context = mockContext.Object
            };

            await hub.JoinRoom("room");

            mockGroups.Verify(groups => groups.AddToGroupAsync("connectionId", "room", default), Times.Once);
        }

        [TestMethod]
        public async Task LeaveRoom_RemovesConnectionFromGroup()
        {
            var mockClients = new Mock<IHubCallerClients>();
            var mockGroups = new Mock<IGroupManager>();
            var mockContext = new Mock<HubCallerContext>();
            mockContext.Setup(m => m.ConnectionId).Returns("connectionId");

            var hub = new MapHub
            {
                Clients = mockClients.Object,
                Groups = mockGroups.Object,
                Context = mockContext.Object
            };

            await hub.LeaveRoom("room");

            mockGroups.Verify(groups => groups.RemoveFromGroupAsync("connectionId", "room", default), Times.Once);
        }
    }
}