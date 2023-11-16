using Demograzy.BusinessLogic.DataAccess;
using Demograzy.Core.Test.CommonRoutines;
using static Demograzy.Core.Test.GeneralConstants;


namespace Demograzy.Core.Test.Candidate.Create.Success
{
    [TestFixture]
    public class CommonAddition
    {


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenAddCandidatesThenReturnsDistinctIds()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("room_owner");
            var roomId = (await service.AddRoomAsync(ownerId, "room", "")).Value;
            var candidateIds = new List<int>();

            for(int i = 0; i < MAX_CANDIDATES; i++)
            {
                var id = (await service.AddCandidateAsync(roomId, $"candidate_{i}")).Value;
                candidateIds.Add(id);
            }

            Assert.That(candidateIds, Is.Unique);
        }


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenAddCandidatesThenCanQueryEachCandidateInfo()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("room_owner");
            var roomId = (await service.AddRoomAsync(ownerId, "room", "")).Value;
            var candidateIds = new List<int>();
            var candidateInfos = new Dictionary<int, CandidateInfo>();

            for(int i = 0; i < MAX_CANDIDATES; i++)
            {
                var info = new CandidateInfo()
                {
                    name = $"candidate_{i}",
                    roomId = roomId
                };
                var id = (await service.AddCandidateAsync(info.roomId, info.name)).Value;
                candidateIds.Add(id);
                candidateInfos.Add(id, info);
            }

            foreach (var id in candidateIds)
            {
                Assert.That(await service.GetCandidateInfo(id), Is.EqualTo(candidateInfos[id]));
            }
        }


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenAddCandidatesThenCandidatesListContainsThem()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("room_owner");
            var roomId = (await service.AddRoomAsync(ownerId, "room", "")).Value;
            var candidateIds = new List<int>();

            for(int i = 0; i < MAX_CANDIDATES; i++)
            {
                var id = (await service.AddCandidateAsync(roomId,  $"candidate_{i}")).Value;
                candidateIds.Add(id);
            }

            Assert.That(await service.GetCandidatesAsync(roomId), Is.EquivalentTo(candidateIds));
        }

    }
}