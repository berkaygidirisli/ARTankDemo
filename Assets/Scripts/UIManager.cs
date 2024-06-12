using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    public TextMeshProUGUI destroyedTankCountText;
    public TextMeshProUGUI tankCountText;
    
    public Button fireButton;
    public Button resetButton;
    public Button createTankButton;
    
    public VariableJoystick movementJoystick;
    public VariableJoystick rotationJoystick;

    private void Start()
    {
        fireButton.onClick.AddListener(() =>
        {
            GameManager.instance.selectedTank.Fire();
        });
        
        resetButton.onClick.AddListener(() =>
        {
            GameManager.instance.ResetGame();
        });
        
        createTankButton.onClick.AddListener(() =>
        {
            GameManager.instance.SpawnTank();
        });
    }


    public void UpdateUI()
    {
        tankCountText.text = "Tank Count: " + GameManager.instance.tankCount;
        destroyedTankCountText.text = "Destroyed Tank Count: " + GameManager.instance.deadTankCount;
    }
}
