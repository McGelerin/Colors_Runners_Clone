using TMPro;
using UnityEngine;

namespace Commands
{
    public class SetVisibilityOfScore
    {
        #region Private Variables

        private TextMeshPro _scoreTMP, _spriteTMP;
        private GameObject _textPlane;

        #endregion

        public SetVisibilityOfScore(ref TextMeshPro scoreTMP, ref TextMeshPro spriteTMP, ref GameObject textPlane)
        {
            _scoreTMP = scoreTMP;
            _spriteTMP = spriteTMP;
            _textPlane = textPlane;
        }

        public void Execute(bool isOpen)
        {
            _scoreTMP.GetComponent<MeshRenderer>().enabled = isOpen;
            _spriteTMP.GetComponent<MeshRenderer>().enabled = isOpen;
            _textPlane.GetComponent<MeshRenderer>().enabled = isOpen;
        }
    }
}