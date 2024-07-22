using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class GameBoard : Node
{
	private readonly List<Card> playerOneCards = [];
	private readonly List<Card> playerTwoCards = [];

	private readonly List<Card> playerOneHand = [];
	private readonly List<Card> playerTwoHand = [];

	private int playerOneHealth = 20;
	private int playerTwoHealth = 20;

	private async void PlayGame()
	{
		// Draw 5 cards for each player
		for (int i = 0; i < 5; i++)
		{
			playerOneHand.Add(playerOneCards[0]);
			playerOneCards.RemoveAt(0);

			playerTwoHand.Add(playerTwoCards[0]);
			playerTwoCards.RemoveAt(0);
		}

		playerOneHand.ForEach(c => c.Position = new Vector2(0, 0));
		playerTwoHand.ForEach(c => c.Position = new Vector2(1000, 1000));

		playerOneHand.ForEach(c => c.Position = new Vector2(500, 0));
		playerTwoHand.ForEach(c => c.Position = new Vector2(500, 1000));

		while (playerOneHealth > 0 && playerTwoHealth > 0)
		{
			// Player One Turn
			var playerOneCard = playerOneCards[0];
			playerOneCards[0].Position = new Vector2(500, 250);
			playerOneCards.RemoveAt(0);
			playerOneHealth -= playerOneCard.Cost;
			playerTwoHealth -= playerOneCard.Power;

			// Player Two Turn
			var playerTwoCard = playerTwoCards[0];
			playerTwoCards[0].Position = new Vector2(500, 750);
			playerTwoCards.RemoveAt(0);
			playerTwoHealth -= playerTwoCard.Cost;
			playerOneHealth -= playerTwoCard.Power;

			GD.Print("Player One Health: " + playerOneHealth);
			GD.Print("Player Two Health: " + playerTwoHealth);

			//wait for 5 second
			await ToSignal(GetTree().CreateTimer(5), "timeout");
		}

		if (playerOneHealth <= 0)
		{
			GD.Print("Player Two Wins!");
		}
		else
		{
			GD.Print("Player One Wins!");
		}
	}

	public override void _Ready()
	{
		Random random = new();
		for (int i = 0; i < 30; i++)
		{
			var card = new Card { Cost = random.Next(1, 10), Power = random.Next(1, 10) };
			playerOneCards.Add(card);

			card = new Card { Cost = random.Next(1, 10), Power = random.Next(1, 10) };
			playerTwoCards.Add(card);
		}

		playerOneCards.ForEach(c => AddChild(c));
		playerTwoCards.ForEach(c => AddChild(c));

		GD.Print("Player One Card average cost: " + playerOneCards.Average(c => c.Cost / c.Power));
		GD.Print("Player Two Card average cost: " + playerTwoCards.Average(c => c.Cost / c.Power));

		//display all cards
		playerOneCards.ForEach(c => GD.Print("Player One Card: " + c.Cost + " " + c.Power));
		playerTwoCards.ForEach(c => GD.Print("Player Two Card: " + c.Cost + " " + c.Power));

		PlayGame();
	}

	public override void _Process(double delta) { }
}
