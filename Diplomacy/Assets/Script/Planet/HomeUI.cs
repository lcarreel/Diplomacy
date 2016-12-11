using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HomeUI : MonoBehaviour {
    
    public Text civilNumber;

    [SerializeField]
    private Slider moodSlider;
    [SerializeField]
    private Image backgroundMoodSlider;

    [SerializeField]
    private Slider powrSlider;
    [SerializeField]
    private Image backgroundPowrSlider;
    [SerializeField]
    private Slider ironSlider;
    [SerializeField]
    private Image backgroundIronSlider;
    [SerializeField]
    private Slider foodSlider;
    [SerializeField]
    private Image backgroundFoodSlider;



    public void ChangeValue(Vector3 supply, int mood, Vector3 supplyNeeded)
    {
        foodSlider.maxValue = 2 * supplyNeeded.x;
        foodSlider.value = supply.x;
        if (supply.x < supplyNeeded.x)
        {
            backgroundFoodSlider.color = Color.red;
        }
        else
        {
            backgroundFoodSlider.color = Color.white;
        }
        
        ironSlider.maxValue = 2 * supplyNeeded.y;
        ironSlider.value = supply.y;
        if (supply.y < supplyNeeded.y)
        {
            backgroundIronSlider.color = Color.red;
        }
        else
        {
            backgroundIronSlider.color = Color.white;
        }
        
        powrSlider.maxValue = 2 * supplyNeeded.z;
        powrSlider.value = supply.z;
        if (supply.z < supplyNeeded.z)
        {
            backgroundPowrSlider.color = Color.red;
        }
        else
        {
            backgroundPowrSlider.color = Color.white;
        }
        
        moodSlider.maxValue = 100;
        moodSlider.value = (float)mood;
        if (mood < 40 )
        {
            backgroundMoodSlider.color = Color.red;
        }
        else if(mood > 60)
        {
            backgroundMoodSlider.color = Color.white;
        } else
        {
            backgroundMoodSlider.color = Color.yellow+Color.red;
        }
    }



}
