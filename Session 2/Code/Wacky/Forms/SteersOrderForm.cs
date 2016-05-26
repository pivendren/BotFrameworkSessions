using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using Wacky.Forms;

namespace Wacky.Forms
{
    public enum BurgerType
    {
        Chicken, Beef, Veg
    }

    public enum Drink
    {
        Coke,
        Fanta,
        Sprite,
        Tab
    }

    public enum Extras
    {
        Salt,
        Pepper,
        Sauce,
        Cheese,
        Patty
    }
}

[Serializable]
public class SteersOrderForm
{
    public BurgerType? Burgertype;
    public Drink? Drink;
    public List<Extras> Extras;

    public static IForm<SteersOrderForm> BuildForm()
    {
        return new FormBuilder<SteersOrderForm>()
            .Message("Wacky Wednesday order please.")
           .Build();
    }
}