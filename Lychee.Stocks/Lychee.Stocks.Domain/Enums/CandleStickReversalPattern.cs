namespace Lychee.Stocks.Domain.Enums
{
    public enum CandleStickReversalPattern
    {
        None = 0,

        //Bearish
        HangingMan = 200, 
        EveningStarDoji = 201,
        BullishEngulping,
        BearishEngulping,
        PiercingLine,


        //Bullish
        Hammer = 100,
    }
}
