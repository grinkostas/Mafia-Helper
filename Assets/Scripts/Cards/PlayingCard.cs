using System;

[Serializable]
public class PlayingCard
{
    public int Id = 0;
    public string RoleName;
    public string RoleDescription;

    public CardType CardType;
    public string ActionText;
    public int TurnOrder;

    public PlayingCard(string name = "Role Name", string roleDescription = "role",
        CardType cardType = CardType.Inactive,string actionText = "action", int turnOrder = 1)
    {
        RoleName = name;
        RoleDescription = roleDescription;
        CardType = cardType;
        ActionText = actionText;
        TurnOrder = turnOrder;
    }

    public string GetLoggedText(Player playerNumber)
    {
        return $"{RoleName} - {ActionText} игрока №{playerNumber}";
    }
    
}
