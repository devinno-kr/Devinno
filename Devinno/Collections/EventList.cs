using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Collections
{
    public class EventList<T> : List<T>
    {
        /// <summary>
        /// 리스트 내용 변경 시 발생하는 이벤트
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// 아이템 추가
        /// </summary>
        /// <param name="item">아이템</param>
        public new void Add(T item) { base.Add(item); Changed?.Invoke(this, new EventArgs()); }
        
        /// <summary>
        /// 아이템 컬렉션 추가
        /// </summary>
        /// <param name="collection">아이템 컬렉션</param>
        public new void AddRange(IEnumerable<T> collection) { base.AddRange(collection); Changed?.Invoke(this, new EventArgs()); }
        
        /// <summary>
        /// 리스트 비우기
        /// </summary>
        public new void Clear() { base.Clear(); Changed?.Invoke(this, new EventArgs()); }

        /// <summary>
        /// 지정한 인덱스에 아이템 삽입
        /// </summary>
        /// <param name="index">인덱스</param>
        /// <param name="item">아이템</param>
        public new void Insert(int index, T item) { base.Insert(index, item); Changed?.Invoke(this, new EventArgs()); }
        
        /// <summary>
        /// 지정한 인덱스에 아이템 컬렉션 삽입
        /// </summary>
        /// <param name="index">인덱스</param>
        /// <param name="collection">아이템 컬렉션</param>        
        public new void InsertRange(int index, IEnumerable<T> collection) { base.InsertRange(index, collection); Changed?.Invoke(this, new EventArgs()); }

        /// <summary>
        /// 아이템 삭제
        /// </summary>
        /// <param name="item">아이템</param>
        /// <returns>삭제 여부</returns>
        public new bool Remove(T item) { var ret = base.Remove(item); Changed?.Invoke(this, new EventArgs()); return ret; }
        
        /// <summary>
        /// 조건의 일치하는 아이템 삭제
        /// </summary>
        /// <param name="match">조건</param>
        /// <returns>제거된 아이템 수</returns>
        public new int RemoveAll(Predicate<T> match) { var ret = base.RemoveAll(match); Changed?.Invoke(this, new EventArgs()); return ret; }
        
        /// <summary>
        /// 지정한 인덱스의 아이템 삭제
        /// </summary>
        /// <param name="index">인덱스</param>
        public new void RemoveAt(int index) { base.RemoveAt(index); Changed?.Invoke(this, new EventArgs()); }
        
        /// <summary>
        /// 지정한 범위의 아이템 삭제
        /// </summary>
        /// <param name="index">인덱스</param>
        /// <param name="count">개수</param>
        public new void RemoveRange(int index, int count) { base.RemoveRange(index, count); Changed?.Invoke(this, new EventArgs()); }

        /// <summary>
        /// 지정한 범위의 아이템 순서 반전
        /// </summary>
        /// <param name="index">인덱스</param>
        /// <param name="count">개수</param>
        public new void Reverse(int index, int count) { base.Reverse(index, count); Changed?.Invoke(this, new EventArgs()); }
        
        /// <summary>
        /// 리스트의 아이템 순서 반전
        /// </summary>
        public new void Reverse() { base.Reverse(); Changed?.Invoke(this, new EventArgs()); }

        /// <summary>
        /// 지정한 범위의 비교자를 기준으로 아이템 정렬 
        /// </summary>
        /// <param name="index">인덱스</param>
        /// <param name="count">범위</param>
        /// <param name="comparer">비교자</param>
        public new void Sort(int index, int count, IComparer<T> comparer) { base.Sort(index, count, comparer); Changed?.Invoke(this, new EventArgs()); }

        /// <summary>
        /// 비교 대리자를 기준으로 아이템 정렬
        /// </summary>
        /// <param name="comparison">Comparison<T> 대리자</param>
        public new void Sort(Comparison<T> comparison) { base.Sort(comparison); Changed?.Invoke(this, new EventArgs()); }
        
        /// <summary>
        /// 아이템 정렬
        /// </summary>
        public new void Sort() { base.Sort(); Changed?.Invoke(this, new EventArgs()); }

        /// <summary>
        /// 비교자를 기준으로 아이템 정렬
        /// </summary>
        /// <param name="comparer">비교자</param>
        public new void Sort(IComparer<T> comparer) { base.Sort(comparer); Changed?.Invoke(this, new EventArgs()); }
    }
}
