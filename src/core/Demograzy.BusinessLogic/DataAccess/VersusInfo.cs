namespace Demograzy.BusinessLogic.DataAccess
{
    public struct VersusInfo
    {
        public int roomId;
        public int firstCandidateId;
        public int secondCandidateId;
        public Statuses status;

        public enum Statuses 
        {
            UNCOMPLETED,
            FIRST_WON,
            SECOND_WON
        }
    }

    
}