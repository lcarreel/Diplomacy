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

    private Animator _animMood;
    private Animator _animPowr;
    private Animator _animIron;
    private Animator _animFood;




    public void ChangeValue(Vector3 supply, int mood, Vector3 supplyNeeded)
    {
        if (_animMood == null)
        {
            _animMood = moodSlider.GetComponent<Animator>();
            _animPowr = powrSlider.GetComponent<Animator>();
            _animIron = ironSlider.GetComponent<Animator>();
            _animFood = foodSlider.GetComponent<Animator>();
}
        foodSlider.maxValue = 2 * supplyNeeded.x;
        foodSlider.value = supply.x;
        if (supply.x < supplyNeeded.x)
        {
            _animFood.SetBool("Danger",true);
        }
        else
        {
            _animFood.SetBool("Danger", false);
        }
        
        ironSlider.maxValue = 2 * supplyNeeded.y;
        ironSlider.value = supply.y;
        if (supply.y < supplyNeeded.y)
        {
            _animIron.SetBool("Danger", true);
        }
        else
        {
            _animIron.SetBool("Danger", false);
        }
        
        powrSlider.maxValue = 2 * supplyNeeded.z;
        powrSlider.value = supply.z;
        if (supply.z < supplyNeeded.z)
        {
            _animPowr.SetBool("Danger", true);

        }
        else
        {
            _animPowr.SetBool("Danger", false);
        }

        moodSlider.maxValue = 100;
        moodSlider.value = (float)mood;
        if (mood < 40 )
        {
            backgroundMoodSlider.color = Color.white;
            _animMood.SetBool("Danger", true);
        }
        else if(mood > 60)
        {
            backgroundMoodSlider.color = Color.white;
            _animMood.SetBool("Danger", false);
        }
        else
        {
            //   backgroundMoodSlider.color = Color.yellow+Color.red;
            _animMood.SetBool("Danger", true);
        }
    }



}
