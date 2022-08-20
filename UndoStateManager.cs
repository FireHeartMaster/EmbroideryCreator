using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbroideryCreator
{
    public class UndoStateManager
    {
        LinkedList<ImageAndOperationsData> undoStates = new LinkedList<ImageAndOperationsData>();
        LinkedListNode<ImageAndOperationsData> currentState = null;

        int maximumUndoSize = 20;

        public bool HasPreviousState()
        {
            return currentState?.Previous != null;
        }

        public bool HasNextState()
        {
            return currentState?.Next != null;
        }

        public void AddNewState(ImageAndOperationsData newState)
        {
            if(undoStates.Count > 0 && currentState != null)
            {
                while (undoStates.Last != currentState)
                {
                    undoStates.RemoveLast();
                }
            }

            undoStates.AddLast(new ImageAndOperationsData(newState));
            currentState = undoStates.Last;

            if(undoStates.Count > maximumUndoSize)
            {
                undoStates.RemoveFirst();
            }
        }

        public ImageAndOperationsData Undo()
        {
            if(currentState.Previous != null)
            {
                currentState = currentState.Previous;
            }            
            return new ImageAndOperationsData(currentState.Value);
        }

        public ImageAndOperationsData Redo()
        {
            if (currentState.Next != null)
            {
                currentState = currentState.Next;
            }
            return new ImageAndOperationsData(currentState.Value);
        }

        public void Reset()
        {
            undoStates = new LinkedList<ImageAndOperationsData>();
            currentState = null;
        }
    }
}
