using UnityEngine;
using TMPro;

public class Validate : MonoBehaviour
{
    public TMP_InputField cardNumberIF;
    public TMP_Text cardNameText;

    private long cardNumber;
    private long numberLenght;

    public void ValidateButton()
    {
        if(long.TryParse(cardNumberIF.text, out cardNumber))
        {
            if (CheckNumber(cardNumber))
                IdentifyCardIssuer();
            else
            {
                cardNameText.color = Color.red;
                cardNameText.text = "Invalid number";
            }
        }
        else
        {
            cardNameText.color = Color.red;
            cardNameText.text = "Invalid number";
        }
    }


    private bool CheckNumber(long n)
    {
        long originalNumber = n;    // Store the original number
        long totalSum = 0;  // Variable to store the total sum of digits
        long placeHolder = 0; // Temporary variable to hold intermediate values
        bool isFirstIteration = true; // Flag to indicate if it's the first iteration

        numberLenght = 0;

        while (originalNumber > 0)
        {
            // Eliminate the last digit to start processing from the penultimate one
            if (isFirstIteration)
            {
                originalNumber /= 10;
                isFirstIteration = false;
            }

            // Process each digit according to specified rules
            if ((originalNumber % 10) * 2 < 10)
            {
                // If the result of doubling the digit is less than 10, simply add it to the total sum
                totalSum += (originalNumber % 10) * 2;
                numberLenght++;
            }
            else 
            {
                // If the result of doubling the digit is 10 or more
                placeHolder = (originalNumber % 10) * 2;
                // Add the last digit of the placeholder to the total sum
                totalSum += placeHolder % 10;

                // Divide placeholder by 10 to get the first digit of the doubled value
                placeHolder /= 10;
                // Add the first digit of the doubled value to the total sum
                totalSum += placeHolder % 10;

                numberLenght++;
            }
            originalNumber /= 100;
        }

        // second sum of numbers
        while (n > 0)
        {
            totalSum += (n % 10);
            n /= 100;

            numberLenght++;
        }

        // Check if the total sum modulo 10 equals 0. 
        // If it does, return true, indicating that the number is valid; otherwise, return false.
        return (totalSum % 10 == 0);
    }

    private void IdentifyCardIssuer()
    {
        long digits = get_digits(numberLenght, cardNumber);
        cardNameText.color = Color.black;

        //  detect which bank the card belongs to
        if (numberLenght == 15 && (digits == 34 || digits == 37))
        {
            cardNameText.text = "The card number corresponds to a American Express card.";
        }
        else if (numberLenght == 16 && (digits >= 51 && digits <= 55))
        {
            cardNameText.text = "The card number corresponds to a MasterCard card.";
        }
        else if ((numberLenght == 13 || numberLenght == 16) && digits / 10 == 4)
        {
            cardNameText.text = "The card number corresponds to a Visa card.";
        }
        else
        {
            cardNameText.color = Color.red;
            cardNameText.text = "Invalid card";
        }
    }

    //take the first 2 digits of the card
    long get_digits(long lenght, long number)
    {
        return (long)(number / System.Math.Pow(10, lenght - 2));
    }
}