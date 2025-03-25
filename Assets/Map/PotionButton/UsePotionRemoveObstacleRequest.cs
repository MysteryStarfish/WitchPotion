namespace Map.PotionButton
{
    public class UsePotionRemoveObstacleRequest
    {
        public string PotionID { get; }
        public int ButtonIndex { get; }
        public UsePotionRemoveObstacleRequest(string potionID, int buttonIndex)
        {
            PotionID = potionID;
            ButtonIndex = buttonIndex;
        }
    }
}