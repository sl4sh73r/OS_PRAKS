public class SimpleDataQueue {

    private int head;
    private int tail;
    private volatile int elementsCount;
    private Integer[] myArrayQueue;

    public SimpleDataQueue(int size) {
        myArrayQueue = new Integer[size];
    }

    public void add(Integer element) {
        synchronized (this) {
            while (elementsCount >= 100) {
                try {
                    wait();
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
            myArrayQueue[head] = element;
            elementsCount++;

            if (head == myArrayQueue.length - 1) {
                head = 0;
            } else {
                head++;
            }
            notifyAll();
        }
    }

    public Integer remove() {
        synchronized (this) {
            while (getElementsCount() == 0) {
                try {
                    wait();
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
            Integer value = myArrayQueue[tail];
            myArrayQueue[tail] = null;
            elementsCount--;

            if (tail == myArrayQueue.length - 1) {
                tail = 0;
            } else {
                tail++;
            }

            if (elementsCount <= 80) {
                notifyAll();
            }
            return value;
        }
    }

    public synchronized int getElementsCount() {
        return elementsCount;
    }
}
