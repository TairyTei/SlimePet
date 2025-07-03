public class VariableReplacer
{
    public static string Replace(string input)
    {
         string newText = "";
        int i = 0;
        while(i < input.Length){
            if(input[i] == '$'){
                string variable = "";
                i++;
                while(input[i] != ' ' && input[i] != ',' && input[i] != '.' && input[i] != '?' && input[i] != '!' && i < input.Length){
                    variable += input[i];
                    i++;
                }
                switch(variable){
                    case "pet_name":
                        newText += Pet.Instance.petName;
                    break;
                }
            }
            else{
                newText += input[i];
                i++;
            }
        }
        return newText;
    }
}