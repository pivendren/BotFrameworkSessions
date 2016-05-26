using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;

namespace WackyWednesdayBot.Form
{
    public enum BurgerOptions
    {
        Beef,
        Chicken,
        Veg
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
        Mustard,
        Pepper,
        Salt,
        Cheese,
        Patty
    }

    [Serializable]
    public class SteersOrder
    {
        public BurgerOptions? BurgerOptions;
        public Drink? Drink;
        public List<Extras> Extras;

        public static IForm<SteersOrder> BuildForm()
        {
            return new FormBuilder<SteersOrder>()
                .Message("Wacky Wednesday order please.")
               .Build();
        }
    }
}