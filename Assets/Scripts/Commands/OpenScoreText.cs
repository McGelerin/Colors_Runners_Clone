using TMPro;
using UnityEngine;

namespace Commands
{
    public class OpenScoreText
    {
        #region Private Variables

        private TextMeshPro _scoreTMP,_spriteTMP;
        private GameObject _textPlane;

        #endregion

        public OpenScoreText(ref TextMeshPro scoreTMP,ref TextMeshPro spriteTMP,ref GameObject textPlane)
        {
            _scoreTMP = scoreTMP;
            _spriteTMP = spriteTMP;
            _textPlane = textPlane;
        }

        public void Execute()
        {
            _scoreTMP.GetComponent<MeshRenderer>().enabled = true;
            _spriteTMP.GetComponent<MeshRenderer>().enabled = true;
            _textPlane.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}