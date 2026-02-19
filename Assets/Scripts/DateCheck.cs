using System;
using TMPro;
using UnityEngine;

public class DateCheck : MonoBehaviour
{
    public TextMeshProUGUI admissionDate;
    public TextMeshProUGUI ageYears;

    void Start()
    {
        AdmissionDate();
        CalculateAge();
    }

    void Update()
    {
        AdmissionDate();
        CalculateAge();
    }

    void AdmissionDate()
    {

        // Get the current system date and time
        DateTime currentDate = DateTime.Now;

        // Subtract one day
        DateTime previousDay = currentDate.AddDays(-1);

        // Display results in the Unity Console
        admissionDate.text = previousDay.ToString("dd/MM/yyyy");

    }

    void CalculateAge()
    {
        
        DateTime currentDate = DateTime.Now;
        DateTime birthDate = new DateTime(1955, 04, 12);

        int fullYears = CalculateFullYears(birthDate, currentDate);
        ageYears.text = ($"{fullYears}");

        int CalculateFullYears(DateTime from, DateTime to)
        {
            if (to < from)
            {
                // Swap if dates are in reverse order
                DateTime temp = from;
                from = to;
                to = temp;
            }

            int years = to.Year - from.Year;

            // Adjust if the last year is incomplete
            if (to.Month < from.Month || (to.Month == from.Month && to.Day < from.Day))
            {
                years--;
            }

            return years;
        }



    }
}
