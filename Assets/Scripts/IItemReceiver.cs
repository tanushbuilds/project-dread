public interface IItemReceiver
{
    bool CanReceive(Grabbable item);
    void ReceiveItem(Grabbable item);
}
