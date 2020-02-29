using UnityEngine;
using UnityEditor;
using System.Collections;

// 이 부분은 에디터 건드는건데 밑에있는 숫자중 14,9는 가로세로 말하는거고.. 18f가 왜 나오는거지? 
// 저거 18 변경하면서 인스펙터창 보니까 체크박스 위아래 간격임 ㅋ
[CustomPropertyDrawer(typeof(ArrayLayout))]
public class CustPropertyDrawer : PropertyDrawer 
{
	public override void OnGUI(Rect position,SerializedProperty property,GUIContent label){
		EditorGUI.PrefixLabel(position,label);
		Rect newposition = position;
		//처음에 18띄우고 체크박스 그리드 시작하는거 
		newposition.y += 18f;
		SerializedProperty data = property.FindPropertyRelative("rows");
        if (data.arraySize != 5)
            data.arraySize = 5;
		//data.rows[0][]
		//14개 만들거임
		for(int j=0;j<5;j++){
			SerializedProperty row = data.GetArrayElementAtIndex(j).FindPropertyRelative("row");
			newposition.height = 18;
			if(row.arraySize != 5)
				row.arraySize = 5;
			newposition.width = position.width/5;
			for(int i=0;i<5;i++){
				EditorGUI.PropertyField(newposition,row.GetArrayElementAtIndex(i),GUIContent.none);
				newposition.x += newposition.width;
			}

			newposition.x = position.x;
			newposition.y += 18f;
		}
	}

	//이거 만드려는 프로퍼티의 높이 지정하는거 같은데
	public override float GetPropertyHeight(SerializedProperty property,GUIContent label){
		return 7f * 15;
	}
}
