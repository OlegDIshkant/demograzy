using Demograzy.Core.Test.CommonRoutines;
using static Demograzy.Core.Test.GeneralConstants;


namespace Demograzy.Core.Test.Candidate.Create.Fail
{
    [TestFixture]
    public class AboveLimit
    {


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenAddCandidateAboveLimitThenReturnsNullId()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("room_owner");
            var roomId = (await service.AddRoomAsync(ownerId, "room", "")).Value;
            for(int i = 0; i < MAX_CANDIDATES; i++)
            {
                await service.AddCandidateAsync(roomId, $"candidate_{i}");
            }

            var idToFail = await service.AddCandidateAsync(roomId, "candidate_to_fail");

            Assert.That(idToFail, Is.Null);
        }


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenAddCandidateAboveLimitThenCandidatesListDoesNotChange()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("room_owner");
            var roomId = (await service.AddRoomAsync(ownerId, "room", "")).Value;
            for(int i = 0; i < MAX_CANDIDATES; i++)
            {
                await service.AddCandidateAsync(roomId, $"candidate_{i}");
            }
            var expectedCandidates = await service.GetCandidatesAsync(roomId);

            await service.AddCandidateAsync(roomId, "candidate_to_fail");

            Assert.That(await service.GetCandidatesAsync(roomId), Is.EqualTo(expectedCandidates));
        }



    }
}