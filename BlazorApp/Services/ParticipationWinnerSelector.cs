﻿using SharedModels.Models.EventParticipants;

namespace BlazorApp.Services;


public class ParticipationWinnerSelector
{
    private readonly Random _random;

    public ParticipationWinnerSelector()
    {
        _random = new Random(DateTime.Now.Microsecond);
    }

    public void MarkParticipantsAsWinners(IReadOnlyList<Participant> participants, bool includeHosts, int numberOfWinnersToDraw)
    {
        SetWinnerState(participants, false);
        var possibleWinners = participants;
        if (!includeHosts)
        {
            possibleWinners = participants.Where(x => !x.EventHost).ToList();
        }

        if (numberOfWinnersToDraw > participants.Count)
        {
            SetWinnerState(possibleWinners, true); //Oprah Mode! Everyone is a winner :-)
            return;
        }

        var winners = new List<Participant>();
        while (winners.Count < numberOfWinnersToDraw)
        {
            var newWinner = possibleWinners[_random.Next(possibleWinners.Count)];
            if (!winners.Contains(newWinner))
            {
                winners.Add(newWinner);
            }
        }

        SetWinnerState(winners, true);

        void SetWinnerState(IReadOnlyList<Participant> participantsToAffect, bool isWinner)
        {
            foreach (var participant in participantsToAffect)
            {
                participant.Winner = isWinner;
            }
        }
    }
}