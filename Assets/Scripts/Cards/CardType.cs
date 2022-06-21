public enum CardType
{
    Don,
    Mafia,
    Active,
    Inactive
}

public static class CardTypeExtension
{
    public static string GetTypeName(this CardType cardType)
    {
        string result;
        switch (cardType)
        {
            case CardType.Don:
                result = "Дон";
                break;
            case CardType.Mafia:
                result = "Мафия роль";
                break;
            case CardType.Active:
                result = "Активная роль";
                break;
            default:
                result = "Не активная роль";
                break;
        }
        return result;
    }
}
