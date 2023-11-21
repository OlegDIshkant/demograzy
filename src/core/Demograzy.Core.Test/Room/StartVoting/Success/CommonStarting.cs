using Demograzy.Core.Test.CommonRoutines;
using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Room.StartVoting.Success
{
    [TestFixture]
    public class CommonStarting
    {
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(16)]
        [TestCase(32)]
        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenStartVotingThenReturnsTrue(int candidatesAmount)
        {
            Assert.That(candidatesAmount >= MIN_CANDIDATES);
            var service = StartUpRoutines.PrepareMainService();
            var owner = await service.AddClientAsync("room_owner"); 
            var room = (await service.AddRoomAsync(owner, "some_room", "")).Value;
            // Add candidates
            var candidates = Enumerable.Range(0, candidatesAmount)
            .Select(async i => await service.AddCandidateAsync(room, $"candidate_{i}"))
            .ToList();

            var startingSucceeded = await service.StartVotingAsync(room);

            Assert.That(startingSucceeded);
        }


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenStartVotingThenNoWinnerYet()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("room_owner"); 
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            var candidate1 = await service.AddCandidateAsync(roomId, "c1");
            var candidate2 = await service.AddCandidateAsync(roomId, "c2");
            
            Assert.That(await service.StartVotingAsync(roomId));

            Assert.That(await service.GetWinnerAsync(roomId), Is.Null);
        }


        [TestCase(2)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(9)]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenStartVotingThenActiveVersesListsHaveCertainLength(int candidatesAmount)
        {
            var service = StartUpRoutines.PrepareMainService();
            // Add room with owner
            var owner = await service.AddClientAsync("room_owner"); 
            var room = (await service.AddRoomAsync(owner, "some_room", "")).Value;
            // Add extra members
            var extraMember1 = await service.AddClientAsync("extraMember1");
            var extraMember2 = await service.AddClientAsync("extraMember2");
            await service.AddMember(room, extraMember1);
            await service.AddMember(room, extraMember2);
            // Add candidates
            var candidates = Enumerable.Range(0, candidatesAmount)
            .Select(async i => (await service.AddCandidateAsync(room, $"candidate_{i}")).Value)
            .Select(t => t.Result)
            .ToList();
            
            Assert.That(await service.StartVotingAsync(room));

            var expectedVersesNum = (int)(candidatesAmount / 2);
            Assert.That(await service.GetActiveVersesAsync(room, owner), Has.Count.EqualTo(expectedVersesNum));
            Assert.That(await service.GetActiveVersesAsync(room, extraMember1), Has.Count.EqualTo(expectedVersesNum));
            Assert.That(await service.GetActiveVersesAsync(room, extraMember2), Has.Count.EqualTo(expectedVersesNum));
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenVotingStartedThenNonMemberActiveVersesListIsNull()
        {
            var service = StartUpRoutines.PrepareMainService();
            var nonMember = await service.AddClientAsync("non_member");
            var owner = await service.AddClientAsync("client_for_room");

            var room = (await service.AddRoomAsync(owner, "some_room", "")).Value;

            Assert.That(await service.GetActiveVersesAsync(room, nonMember), Is.Null);
        }

    }
}