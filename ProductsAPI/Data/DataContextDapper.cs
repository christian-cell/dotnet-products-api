﻿using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ProductsAPI.Data;

public class DataContextDapper : DbContext
{
    private readonly IConfiguration _config;

    public DataContextDapper( IConfiguration config )
    {
        _config = config;
    }

    public IEnumerable<T> LoadData<T>(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.Query<T>(sql);
    }

    public T LoadDataSingle<T>(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.QuerySingleOrDefault<T>(sql);
    }

    public bool ExecuteSql(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.Execute(sql) > 0;
    }

    public int ExecuteSqlWithRowCount(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.Execute(sql);
    }

    public bool ExecuteSqlWithParameters(string sql, List<SqlParameter> parameters)
    {
        SqlCommand commandWithParams = new SqlCommand(sql);

        foreach(SqlParameter parameter in parameters)
        {
            commandWithParams.Parameters.Add(parameter);
        }

        SqlConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        dbConnection.Open();

        commandWithParams.Connection = dbConnection;

        int rowsAffected = commandWithParams.ExecuteNonQuery();

        dbConnection.Close();

        return rowsAffected > 0;
    }

    public IEnumerable<T> LoadDataWithParameters<T>(string sql, DynamicParameters parameters)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.Query<T>(sql, parameters);
    }

    public T LoadDataSingleWithParameters<T>(string sql, DynamicParameters parameters)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.QuerySingle<T>(sql, parameters);
    }
}