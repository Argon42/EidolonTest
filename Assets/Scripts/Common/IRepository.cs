public interface IRepository
{
    void SaveText(string key, string value);
    string LoadText(string key);
}