namespace Common
{
    public interface IRepository
    {
        string LoadText(string key);
        void SaveText(string key, string value);
    }
}