﻿
namespace BlApi;
public interface IEngineer
{
    public int Create(BO.Engineer boEngineer);
    public BO.Engineer? Read(int id);
    public IEnumerable<BO.Engineer> ReadAll(BO.Roles role);
    public void Update(BO.Engineer boEngineer);
    public void Delete(int id);



}
