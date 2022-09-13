namespace HC.Patient.Service.IServices
{
    public interface IChatHubService
    {
        void SendToAll(string name, string message);
    }
}
