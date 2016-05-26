using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Wacky.Forms;

namespace Wacky.Forms
{
    public enum Burger
    {
        [Terms(@"Chi\w*", MaxPhrase = 10)]
        Chicken = 1,
        Beef,
        Veg
    }

    public enum Drink
    {
        Coke = 1,
        Fanta,
        Sprite,
        Tab
    }

    public enum Extras
    {
        [Terms("except", "but", "not", "no", "all", "everything")]
        Everything = 1,

        Salt,
        Pepper,
        Sauce,
        Cheese,
        Patty,
        Chips
    }
}

[Serializable]
[Template(TemplateUsage.NotUnderstood,
    "I do not understand \"{0}\".",
    "Try again, I don't get \"{0}\".",
    "Dude that makes no sense.",
    "I am not the smartest bot, simple words please.")]
[Template(TemplateUsage.EnumSelectOne, "Which option of {&} would you like? {||}", ChoiceStyle = ChoiceStyleOptions.PerLine)]
public class SteersOrderForm
{
    [Prompt("What {&} would you like? {||}")]
    public Burger Burger;

    [Optional]
    public Drink Drink;

    //[Optional]
    //public List<Extras> Extras;

    //6 Adding some business logic
    public List<Extras> Extras
    {
        get { return _extras; }
        set
        {
            if (value != null && value.Contains(Wacky.Forms.Extras.Everything))
            {
                _extras = (from Extras extras in Enum.GetValues(typeof(Extras))
                           where extras != Wacky.Forms.Extras.Everything && !value.Contains(extras)
                           select extras).ToList();
            }
            else
            {
                _extras = value;
            }
        }
    }

    private List<Extras> _extras;

    public DateTime? DeliveryTime;
    public string Address;

    public static IForm<SteersOrderForm> BuildForm()
    {
        OnCompletionAsyncDelegate<SteersOrderForm> processOrder = async (context, state) =>
        {
            await context.PostAsync("We are currently processing your order. We will message you the status.");
            await Task.Delay(1000);
            await context.PostAsync("Done");

        };

        return new FormBuilder<SteersOrderForm>()
            .Message("Wacky Wednesday order please.")
            .OnCompletionAsync(processOrder)
            .Build();
    }

    //Broken code :(
    //public static IForm<SteersOrderForm> BuildForm()
    //{
    //    OnCompletionAsyncDelegate<SteersOrderForm> processOrder = async (context, state) =>
    //    {
    //        await context.PostAsync("We are currently processing your order. We will message you the status.");
    //    };

    //    return new FormBuilder<SteersOrderForm>()
    //                .Message("Welcome to the Wacky Wednesday order bot!")
    //                .Field(nameof(Burger))
    //                .Field(nameof(Drink))
    //                .Field(nameof(Extras))
    //                .Message("For extras you have selected {Extras}.")
    //                .Confirm(async (state) =>
    //                {
    //                    var cost = 0.0;
    //                    switch (state.Burger)
    //                    {
    //                        case Burger.Beef: cost = 25.0; break;
    //                        case Burger.Chicken: cost = 20.0; break;
    //                        case Burger.Veg: cost = 15.0; break;
    //                    }
    //                    return new PromptAttribute($"Total for your order is ${cost:F2} is that ok?");
    //                })
    //                .Field(nameof(SteersOrderForm.DeliveryAddress),
    //                    validate: async (state, response) =>
    //                    {
    //                        var result = new ValidateResult { IsValid = true, Value = response };
    //                        var address = (response as string).Trim();
    //                        if (address.Length > 0 && address[0] < '0' || address[0] > '9')
    //                        {
    //                            result.Feedback = "Address must start with a number.";
    //                            result.IsValid = false;
    //                        }
    //                        return result;
    //                    })
    //                .Field(nameof(SteersOrderForm.DeliveryTime), "What time do you want your order delivered? {||}")
    //                .Confirm("Do you want to order your {Burger} and with {[{Extras}]} to be sent to {DeliveryAddress} {?at {DeliveryTime:t}}?")
    //                .AddRemainingFields()
    //                .Message("Thanks for ordering!")
    //                .OnCompletionAsync(processOrder)
    //                .Build();
    //}
}