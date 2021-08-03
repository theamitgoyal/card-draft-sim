public enum CardType
{
    Shiva,
    Vishnu,
    Bhrama,
    Immortality,
    Resource
}

public class Card
{
    private CardType _cardType;
    public CardType Type => _cardType;
    
    public Card(CardType cardType)
    {
        this._cardType = cardType;
    }
}
