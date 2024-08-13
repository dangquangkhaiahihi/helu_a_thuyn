import dataComment from '../../_mocks/dataComment';

export const fetchData=()=>{
    return new Promise((data) =>{
            data([...dataComment]);
    })
}