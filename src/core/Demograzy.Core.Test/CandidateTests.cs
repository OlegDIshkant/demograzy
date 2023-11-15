using Demograzy.BusinessLogic;
using Demograzy.BusinessLogic.DataAccess;
using Microsoft.VisualBasic;

namespace Demograzy.Core.Test
{
    [TestFixture()]
    public class CandidateTests
    {
        public const int MAX_CANDIDATES = 32;
        public const int STANDARD_TIMEOUT = 3000;


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenAddCandidatesThenReturnsDistinctIds()
        {
            var service = CommonRoutines.PrepareMainService();
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
            var service = CommonRoutines.PrepareMainService();
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
            var service = CommonRoutines.PrepareMainService();
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


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenAddCandidateAboveLimitThenReturnsNullId()
        {
            var service = CommonRoutines.PrepareMainService();
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
            var service = CommonRoutines.PrepareMainService();
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



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteSameCandidateAgainThenReturnsFalse()
        {
            var service = CommonRoutines.PrepareMainService();
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
            var service = CommonRoutines.PrepareMainService();
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


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenAddCandidateAfterVotingStartedThenReturnsNullId()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("room_owner");
            var roomId = (await service.AddRoomAsync(ownerId, "room", "")).Value;
            for(int i = 0; i < MAX_CANDIDATES - 1; i++)
            {
                await service.AddCandidateAsync(roomId, $"candidate_{i}");
            }
            Assert.That(await service.StartVotingAsync(roomId));

            var idToFail = await service.AddCandidateAsync(roomId, "candidate_to_fail");

            Assert.That(idToFail, Is.Null);
        }


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenAddCandidateAfterVotingStartedThenCandidatesListDoesNotChange()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("room_owner");
            var roomId = (await service.AddRoomAsync(ownerId, "room", "")).Value;
            for(int i = 0; i < MAX_CANDIDATES - 1; i++)
            {
                await service.AddCandidateAsync(roomId, $"candidate_{i}");
            }
            Assert.That(await service.StartVotingAsync(roomId));
            var expectedCandidates = await service.GetCandidatesAsync(roomId);

            await service.AddCandidateAsync(roomId, "candidate_to_fail");

            Assert.That(await service.GetCandidatesAsync(roomId), Is.EqualTo(expectedCandidates));
        }


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