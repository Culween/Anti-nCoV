import React, { Component } from 'react';

export class FetchData extends Component {
    static displayName = FetchData.name;

    constructor(props) {
        super(props);
        this.state = { forecasts: [], loading: true };
    }

    componentDidMount() {
        this.populateWeatherData();
    }

    static renderForecastsTable(forecasts) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>时间</th>
                        <th>省</th>
                        <th>市</th>
                        <th>微博来源</th>
                        <th>标题</th>
                        <th>内容</th>
                        <th>链接</th>
                    </tr>
                </thead>
                <tbody>
                    {forecasts.map(forecast =>
                        <tr key={forecast.publishDate}>
                            <td>{forecast.publishDate}</td>
                            <td>{forecast.province}</td>
                            <td>{forecast.city}</td>
                            <td>{forecast.rssFeedName}</td>
                            <td>{forecast.title}</td>
                            <td title={forecast.content}>{forecast.content.slice(0, 100)}</td>
                            <td>{forecast.link}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FetchData.renderForecastsTable(this.state.forecasts);

        return (
            <div>
                <h1 id="tabelLabel" >各省市官博消息</h1>
                <p></p>
                {contents}
            </div>
        );
    }

    async populateWeatherData() {
        const response = await fetch('weatherforecast');
        const data = await response.json();
        this.setState({ forecasts: data, loading: false });
    }
}
