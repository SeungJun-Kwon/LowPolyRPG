namespace UnityEngine.UI
{
    /// <summary>
    /// A standard button that sends an event when clicked.
    /// </summary>
    [AddComponentMenu("UI/CustomButton", 30)]
    public class CustomButton : Button
    {
        AudioClip _buttonClick;

        protected CustomButton()
        { }

        protected override void Start()
        {
            _buttonClick = Resources.Load("Sounds/SFX/SFX_ButtonClick") as AudioClip;
            onClick.AddListener(PressSound);
        }

        void PressSound()
        {
            SoundManager.instance.SFXPlay(_buttonClick);
        }
    }
}
