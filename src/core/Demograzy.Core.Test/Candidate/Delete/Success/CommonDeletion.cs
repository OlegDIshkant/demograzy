using static Demograzy.Core.Test.GeneralConstants;


namespace Demograzy.Core.Test.Candidate.Delete.Success
{
    [TestFixture]
    public class CommonDeletion
    {



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteCandidateThenReturnsTrue()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("room_owner");
            var roomId = (await service.AddRoomAsync(ownerId, "room", "")).Value;
            for(int i = 0; i < MAX_CANDIDATES - 1; i++)
            {
                await service.AddCandidateAsync(roomId, $"candidate_{i}");
            }
            var deletedCandidateId = (await service.AddCandidateAsync(roomId, "candidate_to_delete")).Value;

            var deleteSuccess = await service.DeleteCandidateAsync(deletedCandidateId);

            Assert.That(deleteSuccess);
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteCandidateThenCandidatesListDoesNotContainIt()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("room_owner");
            var roomId = (await service.AddRoomAsync(ownerId, "room", "")).Value;
            for(int i = 0; i < MAX_CANDIDATES - 1; i++)
            {
                await service.AddCandidateAsync(roomId, $"candidate_{i}");
            }
            var deletedCandidateId = (await service.AddCandidateAsync(roomId, "candidate_to_delete")).Value;

            await service.DeleteCandidateAsync(deletedCandidateId);

            Assert.That(await service.GetCandidatesAsync(roomId), Has.No.Member(deletedCandidateId));
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteCandidateThenItsInfoQueryReturnsNull()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("room_owner");
            var roomId = (await service.AddRoomAsync(ownerId, "room", "")).Value;
            for(int i = 0; i < MAX_CANDIDATES - 1; i++)
            {
                await service.AddCandidateAsync(roomId, $"candidate_{i}");
            }
            var deletedCandidateId = (await service.AddCandidateAsync(roomId, "candidate_to_delete")).Value;

            await service.DeleteCandidateAsync(deletedCandidateId);

            Assert.That(await service.GetCandidateInfo(deletedCandidateId), Is.Null);
        }


    }
}