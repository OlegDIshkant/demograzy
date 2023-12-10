using System.Diagnostics;
using System.Reflection.Emit;
using Demograzy.BusinessLogic;
using static Demograzy.Core.Test.GeneralConstants;


namespace Demograzy.Core.Test.CommonRoutines
{
    internal static class PrepareStateRoutines
    {


        public static async Task<(int room, int owner)> NewRoomAndOwner(this MainService service)
        {
            var owner = await service.AddClientAsync("room_owner"); 
            var room = (await service.AddRoomAsync(owner, "some_room", PASSPHRASE)).Value;
            return (room, owner);
        }



        /*public static async Task<List<int>> NewExtraMembers(this MainService service, int room, int amount)
        {
            var members = new List<int>();
            for (int i = 0; i < amount; i++)
            {
                var member = await service.AddClientAsync($"extra_member_{i}");
                Assert.True(await service.AddMember(room, member)); 
                members.Add(member);
            }
            return members;
        }*/



        public static async Task<int> NewExtraMember(this MainService service, int room)
        {
                var member = await service.AddClientAsync("extra_member");
                Assert.True(await service.AddMember(room, member, PASSPHRASE)); 
                return member;
        }



        public static async Task<List<int>> NewCandidates(this MainService service, int room, int amount)
        {
            var candidates = new List<int>();
            for (int i = 0; i < amount; i++)
            {
                var candidate = (await service.AddCandidateAsync(room, $"candidate_{i}")).Value;
                candidates.Add(candidate);
            }
            return candidates;
        }



        /*public static async Task<List<int>> MakeVoteInVersus(
            this MainService service,
            int versus, 
            bool voteForFirst, 
            int votesAmount)
        {
            var room = (await service.GetVersusInfoAsync(versus)).roomId;
            var allMembers = await service.GetMembers(room);

            var votesLeft = votesAmount;
            while (votesLeft > 0)
            {
                var memberVoted = false;

                for (int i = 0; i < allMembers.Count; i++)
                {
                    if (await service.VoteAsync(versus, allMembers[i], voteForFirst))
                    {
                        memberVoted = true;
                        break;
                    }
                }

                if (memberVoted)
                {
                    votesLeft--;
                }
                else
                {
                    throw new Exception();
                }
            }

        }*/


    }
}