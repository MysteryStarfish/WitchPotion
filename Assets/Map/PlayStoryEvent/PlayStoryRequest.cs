namespace Map.PlayStoryEvent
{
    public class PlayStoryRequest
    {
        public int _isFinished;
        public string _showWord;

        public PlayStoryRequest(int isFinished, string showWord)
        {
            _isFinished = isFinished;
            _showWord = showWord;
        } 
    }
}