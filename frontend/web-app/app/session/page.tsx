import React from 'react'
import { getSession } from '../actions/authActions'
import Heading from '../components/Heading';
import AuthTest from './AuthTest';
import { getTokenWorkaround } from '../actions/auctionActions';

export default async function Session() {
    const session = await getSession();
    const token = await getTokenWorkaround();

    return (
        <div>
            <Heading title='Session dashboard' subtitle='Session dashboard' center={false} />

            <div className='bg-blue-200 border-2 border-blue-500'>
                <h3 className='text-lg'>Session data</h3>
                <pre>{JSON.stringify(session, null, 2)}</pre>
            </div>

            <div className='mt-4'>
                <AuthTest />
            </div>

            <div className='bg-green-200 border-2 green-blue-500 mt-4'>
                <h3 className='text-lg'>Token data</h3>
                <pre>{JSON.stringify(token, null, 2)}</pre>
            </div>
        </div>
    )
}
