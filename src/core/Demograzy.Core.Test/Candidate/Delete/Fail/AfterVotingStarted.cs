using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Candidate.Delete.Fail
{
    [TestFixture]
    public class AfterVotingStarted
    {
        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteCandidateAfterVotingStartedThenReturnsFalse()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("room_owner");
            var roomId = (await service.AddRoomAsync(ownerId, "room", "")).Value;
            for(int i = 0; i < MAX_CANDIDATES - 1; i++)
            {
                await service.AddCandidateAsync(roomId, $"candidate_{i}");
            }
            var lastCandidateId = (await service.AddCandidateAsync(roomId, "last_candidate")).Value;
            Assert.That(await service.StartVotingAsync(roomId));

            var deleteFailed = !await service.DeleteCandidateAsync(lastCandidateId);

            Assert.That(deleteFailed);
        }


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteCandidateAfterVotingStartedThenCandidatesListDoesNotChange()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("room_owner");
            var roomId = (await service.AddRoomAsync(ownerId, "room", "")).Value;
            for(int i = 0; i < MAX_CANDIDATES - 1; i++)
            {
                await service.AddCandidateAsync(roomId, $"candidate_{i}");
            }
            var lastCandidateId = (await service.AddCandidateAsync(roomId, "last_candidate")).Value;
            Assert.That(await service.StartVotingAsync(roomId));
            var expectedCandidates = await service.GetCandidatesAsync(roomId);

            Assert.That(!await service.DeleteCandidateAsync(lastCandidateId));

            Assert.That(await service.GetCandidatesAsync(roomId), Is.EqualTo(expectedCandidates));
        }

    }
}