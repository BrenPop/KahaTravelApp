import React, { Component } from 'react';

export class Countries extends Component {

    constructor(props) {
        super(props);
        this.state = { countries: [], loading: true };
    }

    componentDidMount() {
        this.populateCountriesData();
    }

    static renderCountriesTable(countries) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Name</th>
                    </tr>
                </thead>
                <tbody>
                    {countries.map(country =>
                        <tr key={country.name}>
                            <td>{country.name}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Countries.renderCountriesTable(this.state.countries);

        return (
            <div>
                <h1 id="tabelLabel" >Countries</h1>
                <p>Fetching countries</p>
                {contents}
            </div>
        );
    }

    async populateCountriesData() {
        const response = await fetch('countries/all');
        const data = await response.json();
        this.setState({ countries: data, loading: false });
    }
}

