using UnityEngine;
using System.Collections;

namespace NTextSize {
	public class TextSize {
		private Hashtable dict; //map character -> width

		private TextMesh textMesh;
		private Renderer renderer;

		public TextSize(TextMesh tm) {
			textMesh = tm;
			renderer = tm.renderer;
			dict = new Hashtable();
			getSpace();
		}

		private void getSpace() {//the space can not be got alone
			string oldText = textMesh.text;

			textMesh.text = "a";
			float aw = renderer.bounds.size.x;
			textMesh.text = "a a";
			float cw = renderer.bounds.size.x - 2 * aw;

			MonoBehaviour.print("char< > " + cw);
			dict.Add(' ', cw);
			dict.Add('a', aw);

			textMesh.text = oldText;
		}

		public float GetTextWidth(string s) {
			char[] charList = s.ToCharArray();
			float w = 0;
			char c;
			string oldText = textMesh.text;

			for (int i = 0; i < charList.Length; i++) {
				c = charList[i];

				if (dict.ContainsKey(c)) {
					w += (float)dict[c];
				} else {
					textMesh.text = "" + c;
					float cw = renderer.bounds.size.x;
					dict.Add(c, cw);
					w += cw;
					//MonoBehaviour.print("char<" + c +"> " + cw);
				}
			}

			textMesh.text = oldText;
			return w;
		}

		public float width { get { return GetTextWidth(textMesh.text); } }
		public float height { get { return renderer.bounds.size.y; } }
	}
}