namespace ThingsToDo.Common.Helpers
{
    public static class CheckPriority
    {

        public static string CheckTaskPriority(int taskPriority)
        {
            string result = "";

            switch (taskPriority)
            {
                case 1:
                    result = "Lowest";
                    break;
                case 2:
                    result = "Low";
                    break;
                case 3:
                    result = "Normal";
                    break;
                case 4:
                    result = "High";
                    break;
                default:
                    result = "Highest";
                    break;
            }

            return result;
        }
    }
}
