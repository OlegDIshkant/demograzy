using Demograzy.Core.Test.CommonRoutines;
using static Demograzy.Core.Test.GeneralConstants;


namespace Demograzy.Core.Test.Candidate.Delete.Fail
{
    [TestFixture]
    public class RepeatedDeletion
    {





        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteSameCandidateAgainThenReturnsFalse()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("room_owner");
            var roomId = (await service.AddRoomAsync(ownerId, "room", "")).Value;
            for(int i = 0; i < MAX_CANDIDATES - 1; i++)
            {
                await service.AddCandidateAsync(roomId, $"candidate_{i}");
            }
            var deletedCandidateId = (await service.AddCandidateAsync(roomId, "candidate_to_delete")).Value;
            Assert.That(await service.DeleteCandidateAsync(deletedCandidateId));

            var deleteFailed = await service.DeleteCandidateAsync(deletedCandidateId);

            Assert.That(deleteFailed);
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteSameCandidateAgainThenCandidatesListDoesNotChange()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("room_owner");
            var roomId = (await service.AddRoomAsync(ownerId, "room", "")).Value;
            for(int i = 0; i < MAX_CANDIDATES - 1; i++)
            {
                await service.AddCandidateAsync(roomId, $"candidate_{i}");
            }
            var deletedCandidateId = (await service.AddCandidateAsync(roomId, "candidate_to_delete")).Value;
            Assert.That(await service.DeleteCandidateAsync(deletedCandidateId));
            var expectedCandidates = await service.GetCandidatesAsync(roomId);

            await service.DeleteCandidateAsync(deletedCandidateId);

            Assert.That(await service.GetCandidatesAsync(roomId), Is.EqualTo(expectedCandidates));
        }





    }
}