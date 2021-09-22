# https://leetcode.com/problems/letter-combinations-of-a-phone-number/
class Solution:
    def searchcombination(self, alph:List[str], n:int)->List[str]:
        res=[]
        for i in range(0, len(alph[n])):
            if(n==len(alph)-1):
                res.append(alph[n][i])
                continue
            for j in self.searchcombination(alph, n+1):
                res.append(alph[n][i]+j)
        return res
    
    def letterCombinations(self, digits: str) -> List[str]:
        if(digits==""):
            return []
        alph=[]
        
        
        for i in digits:
            if(i=="2"):
                alph.append('abc')
            elif (i=="3"):
                alph.append('def')
            elif (i=="4"):
                alph.append('ghi')
            elif (i=="5"):
                alph.append('jkl')
            elif (i=="6"):
                alph.append('mno')
            elif (i=="7"):
                alph.append('pqrs')
            elif (i=="8"):
                alph.append('tuv')
            elif (i=="9"):
                alph.append('wxyz')
                
    
        return self.searchcombination(alph, 0)
