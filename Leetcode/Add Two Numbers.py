# https://leetcode.com/problems/add-two-numbers/
# Definition for singly-linked list.
# class ListNode:
#     def __init__(self, val=0, next=None):
#         self.val = val
#         self.next = next
class Solution:
    def addTwoNumbers(self, l1: ListNode, l2: ListNode) -> ListNode:
        a = l1
        f=""
        while (a!=None):
            a1=a.val
            f+=str(a1)
            a=a.next
        f=f[::-1]

            
        a2 = l2
        f2=""
        while (a2!=None):
            a12=a2.val
            f2+=str(a12)
            a2=a2.next
        f2=f2[::-1]
        
        res=int(f2)+int(f)
        res=[int(x) for x in (list(str(res))[::-1])]
        b=creatingb(res)

        return b
            
def creatingb(res):
    b=ListNode()
    if res==[]:
        return None
    b.val=res[0]
    b.next=creatingb(res[1::])
    return b
    
