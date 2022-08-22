using TMPro;
using UnityEngine;

namespace Commands
{
    public class CloseScoreText
    {
        #region Private Variables

        private TextMeshPro _scoreTMP, _spriteTMP;
        private GameObject _textPlane;

        #endregion

        public CloseScoreText(ref TextMeshPro scoreTMP, ref TextMeshPro spriteTMP, ref GameObject textPlane)
        {
            _scoreTMP = scoreTMP;
            _spriteTMP = spriteTMP;
            _textPlane = textPlane;
        }

        public void Execute()
        {
            _scoreTMP.GetComponent<MeshRenderer>().enabled = false;
            _spriteTMP.GetComponent<MeshRenderer>().enabled = false;
            _textPlane.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}