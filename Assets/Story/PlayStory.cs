using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;

namespace WitchPotion.Story
{
    public class PlayStory : MonoBehaviour
    {
        [SerializeField]
        private Image background;
        [SerializeField]
        private Image leftCharacter;
        [SerializeField]
        private Image rightCharacter;
        [SerializeField]
        private GameObject dialogueBox;

        private TMP_Text dialogueText;

        private void Start()
        {
            this.dialogueText = this.dialogueBox.GetComponentInChildren<TMP_Text>();
            this.resetState();

            // TODO: remove after finishing testing
            this.onHiddenDoorBypassed().Forget();
        }

        public async UniTask onHiddenDoorBypassed()
        {
            // TODO: update images

            await this.showDialogue("門好像怪怪的");
            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
            this.resetState();
        }
        public async UniTask showWord(string s)
        {
            // TODO: update images

            await this.showDialogue(s);
            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
            this.resetState();
        }

        private void resetState()
        {
            this.background.gameObject.SetActive(false);
            this.leftCharacter.gameObject.SetActive(false);
            this.rightCharacter.gameObject.SetActive(false);
            this.dialogueBox.SetActive(false);
            this.dialogueText.text = "";
        }

        private async UniTask showDialogue(string dialogue)
        {
            this.dialogueBox.SetActive(true);
            this.dialogueText.text = "";
            this.dialogueText.maxVisibleCharacters = 0;
            this.dialogueText.text = dialogue;
            for (int i = 0; i < dialogue.Length; i++)
            {
                this.dialogueText.maxVisibleCharacters = i + 1;
                await UniTask.Delay(50);
            }
        }

        private async UniTask showLeftCharacter(Sprite sprite)
        {
            await showCharacterImage(this.leftCharacter, sprite);
        }

        private async UniTask showRightCharacter(Sprite sprite)
        {
            await showCharacterImage(this.rightCharacter, sprite);
        }

        private async UniTask showCharacterImage(Image target, Sprite sprite)
        {
            target.gameObject.SetActive(true);
            target.sprite = sprite;
            target.color = new Color(1, 1, 1, 0);
            await LMotion.Create(new Color(1, 1, 1, 0), Color.white, 0.5f)
                .BindToColor(target);
        }
    }
}
