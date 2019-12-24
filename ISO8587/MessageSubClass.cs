namespace ISO8583
{
    public enum MessageSubClass
    {
        Request = 00,
        Response = 10,
        Advice = 20,
        ResponseToAdvice = 30,
        Notification = 40
    }
}
